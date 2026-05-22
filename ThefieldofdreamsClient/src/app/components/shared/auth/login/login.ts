import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../../services/auth.service';
import { ToastService } from '../../../../services/toast.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-login',
  templateUrl: './login.html',
  standalone: true,  
  imports: [
    CommonModule, 
    ReactiveFormsModule,  
    RouterModule
  ],
  styleUrls: ['./login.css']
})
export class Login implements OnInit {
  mode: 'login' | 'register' = 'login';
  isLoading = false;
  errorMessage = '';

  loginForm!: FormGroup;
  registerForm!: FormGroup;

  // List of disposable email domains
  private disposableDomains = new Set([
    '10minutemail.com', 'mailinator.com', 'guerrillamail.com', 'yopmail.com',
    'tempmail.com', 'trashmail.com', 'spamgourmet.com', 'fakeinbox.com'
  ]);

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.initLoginForm();
    this.initRegisterForm();

    // Check if already logged in
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/dashboard']);
    }
  }

  private initLoginForm(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      rememberMe: [false]
    });
  }

  private initRegisterForm(): void {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email], [this.disposableEmailValidator.bind(this)]],
      phone: [''],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
      accountType: ['player', [Validators.required]],
      gdpr: [false, [Validators.requiredTrue]]
    }, { validators: this.passwordMatchValidator });
  }

  // Custom async validator for disposable emails
  disposableEmailValidator(control: AbstractControl): Promise<ValidationErrors | null> {
    const email = control.value;
    if (!email) return Promise.resolve(null);
    
    const domain = email.split('@')[1]?.toLowerCase();
    if (domain && this.disposableDomains.has(domain)) {
      return Promise.resolve({ disposableEmail: true });
    }
    return Promise.resolve(null);
  }

  // Password match validator
  passwordMatchValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return password === confirm ? null : { mismatch: true };
  }

  setMode(mode: 'login' | 'register'): void {
    this.mode = mode;
    this.errorMessage = '';
  }

  onLogin(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const { email, password, rememberMe } = this.loginForm.value;

    this.authService.login(email, password, rememberMe).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.toastService.show('Welcome back! ✨', 'success');
        
        // Track login event
        this.trackEvent('user_login', { email, method: 'email' });
        
        // Navigate to intended page or dashboard
        const redirectUrl = this.authService.getRedirectUrl() || '/dashboard';
        this.router.navigate([redirectUrl]);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.message || 'Invalid email or password. Please try again.';
        this.trackEvent('login_failed', { email, error: this.errorMessage });
      }
    });
  }

  onRegister(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      this.toastService.show('Please check the form for errors', 'error');
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const formData = this.registerForm.value;
    const userData = {
      firstName: formData.firstName,
      lastName: formData.lastName,
      name: `${formData.firstName} ${formData.lastName}`,
      email: formData.email,
      phone: formData.phone,
      password: formData.password,
      accountType: formData.accountType,
      gdprConsent: formData.gdpr
    };

    this.authService.register(userData).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.toastService.show('Account created! Please check your email to verify.', 'success');
        this.trackEvent('user_register', { email: userData.email, accountType: userData.accountType });
        
        // Show verification popup or switch to login
        if (response.requiresVerification) {
          this.showVerificationModal(userData.email);
        } else {
          this.mode = 'login';
          this.loginForm.patchValue({ email: userData.email });
          this.toastService.show('Account created! You can now sign in.', 'success');
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.message || 'Registration failed. Please try again.';
        this.trackEvent('register_failed', { email: userData.email, error: this.errorMessage });
      }
    });
  }

  onForgotPassword(): void {
    const email = this.loginForm.get('email')?.value;
    if (!email || !this.loginForm.get('email')?.valid) {
      this.toastService.show('Please enter a valid email address first', 'warning');
      return;
    }

    this.authService.requestPasswordReset(email).subscribe({
      next: () => {
        this.toastService.show(`Reset link sent to ${email}. Check your inbox.`, 'success');
        this.trackEvent('password_reset_request', { email });
      },
      error: () => {
        this.toastService.show('Failed to send reset link. Please try again.', 'error');
      }
    });
  }

  socialLogin(provider: 'google' | 'whatsapp'): void {
    this.isLoading = true;
    
    if (provider === 'google') {
      this.authService.googleLogin().subscribe({
        next: (response) => {
          this.isLoading = false;
          const userName = response.user?.name ?? 'User';
          this.toastService.show(`Welcome ${userName}!`, 'success');
          this.trackEvent('social_login', { provider: 'google' });
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = 'Google login failed. Please try again.';
          this.trackEvent('social_login_failed', { provider: 'google', error: err.message });
        }
      });
    } else {
      // WhatsApp login — opens WhatsApp for OTP
      const phone = prompt('Enter your WhatsApp number with country code (e.g., +30 694 123 4567):');
      if (phone) {
        this.authService.whatsappLogin(phone).subscribe({
          next: (response) => {
            this.isLoading = false;
            if (response.otpSent) {
              const otp = prompt('Enter the OTP sent to your WhatsApp:');
              if (otp) {
                this.verifyWhatsAppOtp(phone, otp);
              }
            }
          },
          error: () => {
            this.isLoading = false;
            this.errorMessage = 'WhatsApp login failed. Please try again.';
          }
        });
      } else {
        this.isLoading = false;
      }
    }
  }

  private verifyWhatsAppOtp(phone: string, otp: string): void {
    this.authService.verifyWhatsAppOtp(phone, otp).subscribe({
      next: (response) => {
        this.toastService.show('WhatsApp login successful!', 'success');
        this.trackEvent('social_login', { provider: 'whatsapp' });
        this.router.navigate(['/dashboard']);
      },
      error: () => {
        this.errorMessage = 'Invalid OTP. Please try again.';
      }
    });
  }

  private showVerificationModal(email: string): void {
    // You can implement a modal service here
    this.toastService.show(`Verification email sent to ${email}. Please check your inbox.`, 'info');
  }

  private trackEvent(eventName: string, data: any): void {
    // Send to analytics or webhook
    if (typeof window !== 'undefined') {
      console.log(`[Track] ${eventName}:`, data);
      // Uncomment when API is ready
      // this.http.post('/api/track', { event: eventName, ...data }).subscribe();
    }
  }
}
