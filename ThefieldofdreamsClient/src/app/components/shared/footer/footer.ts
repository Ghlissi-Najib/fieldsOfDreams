import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
interface FooterLink {
  label: string;
  route?: string;
  href?: string;
  icon: string;
}

interface FooterColumn {
  title: string;
  icon: string;
  links: FooterLink[];
}
@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './footer.html',
  styleUrl: './footer.css'
})


export class Footer {
  currentYear = new Date().getFullYear();
  siteName = 'The Field of Dreams';
  location = 'Crete';
  
  quoteText = 'I hope for nothing. I fear nothing. I am free.';
  quoteAuthor = 'Nikos Kazantzakis';

  // Dynamic footer links structure
  footerLinks: FooterColumn[] = [
    {
      title: 'Explore',
      icon: 'bi-compass',
      links: [
        { label: 'Home', route: '/home', icon: 'bi-house' },
        { label: 'Discover', route: '/swipe', icon: 'bi-arrow-left-right' },
        { label: 'Predictions', route: '/predictions', icon: 'bi-graph-up-arrow' },
        { label: 'Leaderboard', route: '/leaderboard', icon: 'bi-trophy' }
      ]
    },
    {
      title: 'Company',
      icon: 'bi-building',
      links: [
        { label: 'About', route: '/about', icon: 'bi-info-circle' },
        { label: 'Sponsors', route: '/sponsors', icon: 'bi-star' },
        { label: 'Squad', route: '/cruise-squad', icon: 'bi-people' },
        { label: 'Privacy', route: '/privacy', icon: 'bi-shield-check' }
      ]
    },
    {
      title: 'Connect',
      icon: 'bi-link-45deg',
      links: [
        { label: 'WhatsApp', href: 'https://wa.me/306942796159', icon: 'bi-whatsapp' },
        { label: 'Facebook', href: 'https://www.facebook.com/cretevc', icon: 'bi-facebook' },
        { label: 'Instagram', href: 'https://www.instagram.com/cretevc', icon: 'bi-instagram' },
        { label: 'Email', href: 'mailto:info@cretevc.com', icon: 'bi-envelope' }
      ]
    }
  ];

  // Social links for bottom bar
  socialLinks = [
    { name: 'WhatsApp', url: 'https://wa.me/306942796159', icon: 'bi-whatsapp' },
    { name: 'Facebook', url: 'https://www.facebook.com/cretevc', icon: 'bi-facebook' },
    { name: 'Instagram', url: 'https://www.instagram.com/cretevc', icon: 'bi-instagram' },
    { name: 'Email', url: 'mailto:info@cretevc.com', icon: 'bi-envelope' }
  ];

  closeMenu(): void {
    // Method for closing mobile menu if needed
  }
}