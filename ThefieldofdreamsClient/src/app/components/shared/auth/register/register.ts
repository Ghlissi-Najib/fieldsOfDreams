import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true, 
  imports: [
    CommonModule, 
    ReactiveFormsModule,  
    RouterModule
  ],    
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {

}
