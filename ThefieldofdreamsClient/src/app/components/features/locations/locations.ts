import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LocationService } from '../../../services/location.service';
import { Location, LocationType } from '../../../models/location';

@Component({
  selector: 'app-locations',
  imports: [CommonModule, FormsModule],
  templateUrl: './locations.html',
  styleUrl: './locations.css',
})
export class Locations implements OnInit {
  locations = signal<Location[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  typeFilter = signal<LocationType | 'All'>('All');

  readonly typeOptions: Array<LocationType | 'All'> = ['All', 'Stadium', 'TouristAttraction', 'Restaurant', 'Hotel', 'SponsorBooth', 'HiddenGem', 'TransportationHub'];

  readonly filteredLocations = computed(() => {
    const filter = this.typeFilter();
    if (filter === 'All') return this.locations();
    return this.locations().filter(l => l.type === filter);
  });

  constructor(private locationService: LocationService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading.set(true);
    this.error.set(null);
    this.locationService.getAll().subscribe({
      next: (data) => {
        this.locations.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Failed to load locations. Please try again.');
        this.isLoading.set(false);
      }
    });
  }

  onTypeFilter(type: LocationType | 'All'): void {
    this.typeFilter.set(type);
  }
}
