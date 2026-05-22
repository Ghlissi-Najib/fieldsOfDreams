import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TourismRouteService } from '../../../services/tourism-route.service';
import { TourismRoute } from '../../../models/tourism-route';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-tourism-routes',
  imports: [CommonModule, FormsModule],
  templateUrl: './tourism-routes.html',
  styleUrl: './tourism-routes.css',
})
export class TourismRoutes implements OnInit {
  routes = signal<TourismRoute[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  startingRouteId = signal<string | null>(null);

  constructor(
    private tourismRouteService: TourismRouteService,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading.set(true);
    this.error.set(null);
    this.tourismRouteService.getAll().subscribe({
      next: (data) => {
        this.routes.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Failed to load tourism routes. Please try again.');
        this.isLoading.set(false);
      }
    });
  }

  startRoute(routeId: string): void {
    const user = this.authService.getCurrentUser();
    if (!user) return;

    this.startingRouteId.set(routeId);
    this.tourismRouteService.startRoute(routeId, user.id).subscribe({
      next: () => {
        this.startingRouteId.set(null);
      },
      error: () => {
        this.error.set('Failed to start route. Please try again.');
        this.startingRouteId.set(null);
      }
    });
  }
}
