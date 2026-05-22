import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './about.html',
  styleUrls: ['./about.css']
})
export class About implements OnInit, AfterViewInit {
  // Dynamic statistics data
  stats = [
    { value: 100, suffix: '%', label: 'LOCAL PARTNERS', desc: 'Only authentic local businesses' },
    { value: 0, suffix: '', label: 'TOURIST TRAPS', desc: 'Zero generic experiences' },
    { value: 100, suffix: '%', label: 'DIRECT', desc: 'No middlemen, no commissions' },
    { value: 500, suffix: '+', label: 'HAPPY TRAVELERS', desc: 'And counting' }
  ];

  // Categories data
  categories = [
    {
      icon: '🍽️',
      badge: 'SAVOR',
      title: 'Curated Gastronomy',
      desc: 'Selected flavors that define the Cretan table. Not tourist menus — real food, real people, real places.',
      class: ''
    },
    {
      icon: '🏔️',
      badge: 'EXPLORE',
      title: 'Discovery & Nature',
      desc: 'Unveiling the island\'s hidden landscapes and untamed beauty — away from the crowds and the obvious.',
      class: ''
    },
    {
      icon: '🤝',
      badge: 'CONNECT',
      title: 'Local Artisans & People',
      desc: 'Meeting the hands and souls that keep our heritage alive. The stories no guidebook tells.',
      class: ''
    }
  ];

  // Local features data
  localFeatures = [
    {
      icon: '🫒',
      title: 'Family Olive Groves',
      desc: 'Three generations of the same family pressing the same olives on the same land.'
    },
    {
      icon: '🍷',
      title: 'Independent Winemakers',
      desc: 'Wine from vineyards where the winemaker pours it for you personally.'
    },
    {
      icon: '🧭',
      title: 'Guides Who Grew Up Here',
      desc: 'They hiked these trails as children. That difference is everything.'
    },
    {
      icon: '🏺',
      title: 'Artisans & Craftspeople',
      desc: 'Skills passed down through families for centuries.'
    }
  ];

  // Philosophy data
  philosophy = [
    {
      number: '01',
      title: 'Intentional Simplicity',
      desc: 'Luxury is clarity. Nothing unnecessary. We remove the noise so the signal comes through.'
    },
    {
      number: '02',
      title: 'Emotional Depth',
      desc: 'Experiences should be felt, not just seen. If it doesn\'t move you, it\'s not worth featuring.'
    },
    {
      number: '03',
      title: 'Quiet Confidence',
      desc: 'No noise. No exaggeration. Just substance. We let the quality speak for itself.'
    }
  ];

  // Values data
  values = [
    {
      title: 'Intentional Simplicity',
      desc: 'Luxury is clarity. Nothing unnecessary. We remove the noise so the signal comes through.'
    },
    {
      title: 'Emotional Depth',
      desc: 'Experiences should be felt, not just seen. If it doesn\'t move you, it\'s not worth featuring.'
    },
    {
      title: 'Quiet Confidence',
      desc: 'No noise. No exaggeration. Just substance. We let the quality speak for itself.'
    },
    {
      title: 'Human Connection',
      desc: 'Every journey begins with a person, not a click. When you message us, a real local answers.'
    }
  ];

  // Apart features
  apartFeatures = [
    {
      title: 'A Curated Beginning',
      desc: 'Where anticipation meets intent. We hand-select every partner, experience, and story on this platform.'
    },
    {
      title: 'A Personal Narrative',
      desc: 'Real stories, not just sites. Every feature is a chapter in your personal story of Crete.'
    },
    {
      title: 'Engaged Rewards',
      desc: 'Loyalty through shared experiences — not points systems. The more you explore, the more Crete reveals itself.'
    },
    {
      title: 'A Reason to Return',
      desc: 'Because Crete is a lifelong discovery. We are not a platform you use once. We are a companion you return to.'
    }
  ];

  ngOnInit(): void {
    // Initialize any data fetching here
  }

  ngAfterViewInit(): void {
    this.startCounters();
  }

  startCounters(): void {
    const counters = document.querySelectorAll('.counter');
    
    const animateCounter = (counter: Element) => {
      const target = parseInt(counter.getAttribute('data-count') || '0');
      let current = 0;
      const increment = target / 50;
      const updateCounter = () => {
        current += increment;
        if (current < target) {
          counter.textContent = Math.ceil(current).toString();
          requestAnimationFrame(updateCounter);
        } else {
          counter.textContent = target.toString();
        }
      };
      updateCounter();
    };

    const observer = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          animateCounter(entry.target);
          observer.unobserve(entry.target);
        }
      });
    }, { threshold: 0.5 });

    counters.forEach(counter => observer.observe(counter));
  }
}