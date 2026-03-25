import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,  
  imports: [CommonModule],
  templateUrl: './header.html',
  styleUrls: ['./header.css'],
})
export class Header {

  constructor(private router: Router) {}

  get isLoggedIn(): boolean {
    return localStorage.getItem('acts-mock-auth') === 'true';
  }

  logout(): void {
    localStorage.removeItem('acts-mock-auth');
    this.router.navigate(['/']);
  } 

}
