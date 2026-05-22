import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../../services/auth.service';
import { ToastService } from '../../../../services/toast.service';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './settings.html',
  styleUrl: './settings.css'
})
export class Settings {
  passwordForm: FormGroup;
  isSaving = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastService: ToastService
  ) {
    this.passwordForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(group: FormGroup) {
    const pw = group.get('newPassword')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return pw === confirm ? null : { mismatch: true };
  }

  onChangePassword(): void {
    if (this.passwordForm.invalid || this.isSaving) return;
    this.isSaving = true;
    // Placeholder until backend exposes a change-password endpoint
    setTimeout(() => {
      this.toastService.show('Password change is not yet available on the server.', 'info');
      this.isSaving = false;
      this.passwordForm.reset();
    }, 500);
  }

  onLogout(): void {
    this.authService.logout(true);
  }
}

