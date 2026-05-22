import { isPlatformBrowser } from '@angular/common';
import { CommonModule } from '@angular/common';
import {
  Component,
  inject,
  OnInit,
  PLATFORM_ID,
  signal,
} from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AccountProfile, UpdateProfileRequest } from '../../../../models/user';
import { AccountService } from '../../../../services/account.service';
import { AuthService } from '../../../../services/auth.service';
import { ToastService } from '../../../../services/toast.service';

@Component({
  selector: 'app-profil',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profil.html',
  styleUrl: './profil.css',
})
export class Profil implements OnInit {
  private readonly accountService = inject(AccountService);
  private readonly authService = inject(AuthService);
  private readonly toastService = inject(ToastService);
  private readonly fb = inject(FormBuilder);
  private readonly platformId = inject(PLATFORM_ID);

  profile = signal<AccountProfile | null>(null);
  isLoading = signal(true);
  isSaving = signal(false);
  error = signal<string | null>(null);

  profileForm!: FormGroup;

  ngOnInit(): void {
    // No browser APIs during SSR — skip all data loading
    if (!isPlatformBrowser(this.platformId)) return;

    this.profileForm = this.fb.group({
      firstName: [''],
      lastName: [''],
      phoneNumber: [''],
      profilePhotoUrl: [''],
    });

    // Pre-populate immediately from the cached auth user so the form isn't blank while loading
    const cached = this.authService.getCurrentUser();
    if (cached) {
      this.profile.set({
        id: cached.id,
        name: cached.name,
        firstName: cached.firstName,
        lastName: cached.lastName,
        email: cached.email,
        phone: cached.phone,
        profilePhotoUrl: cached.profilePhotoUrl,
        accountType: cached.accountType,
        verified: cached.verified,
        isActive: cached.isActive,
        createdAt: cached.createdAt,
        points: cached.points,
        tickets: cached.tickets,
        totalPoints: cached.totalPoints,
        currentLevel: cached.currentLevel,
      });
      this.profileForm.patchValue({
        firstName: cached.firstName ?? '',
        lastName: cached.lastName ?? '',
        phoneNumber: cached.phone ?? '',
        profilePhotoUrl: cached.profilePhotoUrl ?? '',
      });
    }

    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.accountService.getProfile().subscribe({
      next: (data) => {
        this.profile.set(data);
        this.profileForm.patchValue({
          firstName: data.firstName ?? '',
          lastName: data.lastName ?? '',
          phoneNumber: data.phoneNumber ?? data.phone ?? '',
          profilePhotoUrl: data.profilePhotoUrl ?? '',
        });
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Failed to load profile.');
        this.isLoading.set(false);
      },
    });
  }

  onSave(): void {
    if (this.profileForm.invalid || this.isSaving()) return;
    this.isSaving.set(true);

    const request: UpdateProfileRequest = {
      firstName: this.profileForm.value.firstName,
      lastName: this.profileForm.value.lastName,
      profilePhotoUrl: this.profileForm.value.profilePhotoUrl,
    };

    this.accountService.updateProfile(request).subscribe({
      next: () => {
        this.toastService.show('Profile updated successfully!', 'success');
        this.isSaving.set(false);
        this.loadProfile();
      },
      error: () => {
        this.toastService.show('Failed to update profile. Please try again.', 'error');
        this.isSaving.set(false);
      },
    });
  }

  getInitials(): string {
    const p = this.profile();
    if (!p) return '?';
    const first = p.firstName?.[0] ?? '';
    const last = p.lastName?.[0] ?? '';
    const combined = (first + last).toUpperCase();
    return combined || (p.email?.[0]?.toUpperCase() ?? '?');
  }

  getRoleBadgeClass(role: string): string {
    switch (role.toLowerCase()) {
      case 'admin':     return 'role-badge role-badge--admin';
      case 'merchant':  return 'role-badge role-badge--merchant';
      case 'partner':   return 'role-badge role-badge--partner';
      default:          return 'role-badge';
    }
  }
}
