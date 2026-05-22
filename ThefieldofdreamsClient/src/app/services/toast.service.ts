import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({ providedIn: 'root' })
export class ToastService {
  constructor() {}

  /**
   * Show success toast
   */
  success(message: string = 'Success!'): void {
    Swal.fire({
      icon: 'success',
      title: message,
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      background: '#0e0e1c',
      color: '#eeeef5',
      iconColor: '#00d97e'
    });
  }

  /**
   * Show error toast
   */
  error(message: string = 'An error occurred'): void {
    Swal.fire({
      icon: 'error',
      title: message,
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 4000,
      timerProgressBar: true,
      background: '#0e0e1c',
      color: '#eeeef5',
      iconColor: '#ff4560'
    });
  }

  /**
   * Show warning toast
   */
  warning(message: string = 'Warning'): void {
    Swal.fire({
      icon: 'warning',
      title: message,
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 3500,
      timerProgressBar: true,
      background: '#0e0e1c',
      color: '#eeeef5',
      iconColor: '#f0b429'
    });
  }

  /**
   * Show info toast
   */
  info(message: string = 'Info'): void {
    Swal.fire({
      icon: 'info',
      title: message,
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      background: '#0e0e1c',
      color: '#eeeef5',
      iconColor: '#00cfff'
    });
  }

  /**
   * Show a toast with custom options
   */
  show(message: string, type: 'success' | 'error' | 'warning' | 'info' = 'info'): void {
    const config = {
      success: { icon: 'success', color: '#00d97e', timer: 3000 },
      error: { icon: 'error', color: '#ff4560', timer: 4000 },
      warning: { icon: 'warning', color: '#f0b429', timer: 3500 },
      info: { icon: 'info', color: '#00cfff', timer: 3000 }
    };

    const { icon, color, timer } = config[type];

    Swal.fire({
      icon: icon as any,
      title: message,
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: timer,
      timerProgressBar: true,
      background: '#0e0e1c',
      color: '#eeeef5',
      iconColor: color
    });
  }

  /**
   * Show a toast that stays until dismissed
   */
  persistent(message: string, type: 'success' | 'error' | 'warning' | 'info' = 'info'): void {
    const config = {
      success: { icon: 'success', color: '#00d97e' },
      error: { icon: 'error', color: '#ff4560' },
      warning: { icon: 'warning', color: '#f0b429' },
      info: { icon: 'info', color: '#00cfff' }
    };

    const { icon, color } = config[type];

    Swal.fire({
      icon: icon as any,
      title: message,
      toast: true,
      position: 'top-end',
      showConfirmButton: true,
      confirmButtonText: 'Dismiss',
      confirmButtonColor: color,
      timer: 0,
      background: '#0e0e1c',
      color: '#eeeef5',
      iconColor: color
    });
  }

  /**
   * Show loading toast (with no auto-dismiss)
   * Returns a reference that can be closed later
   */
  loading(message: string = 'Loading...'): any {
    return Swal.fire({
      title: message,
      toast: true,
      position: 'top-end',
      showConfirmButton: false,
      timer: 0,
      didOpen: () => {
        Swal.showLoading();
      },
      background: '#0e0e1c',
      color: '#eeeef5'
    });
  }

  /**
   * Close the current toast
   */
  close(): void {
    Swal.close();
  }
}