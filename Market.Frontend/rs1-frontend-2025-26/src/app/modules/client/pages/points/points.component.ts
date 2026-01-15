import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-points',
  templateUrl: './points.component.html',
  styleUrls: ['./points.component.scss']
})
export class PointsComponent implements OnInit {

  userFullName: string = '';

  ngOnInit(): void {
    const userJson = localStorage.getItem('auth_user');

    if (userJson) {
      const user = JSON.parse(userJson);

      // ✅ 1. Ako postoji fullName – koristi njega
      if (user.fullName && user.fullName.trim() !== '') {
        this.userFullName = user.fullName;
      }
      // ✅ 2. Ako nema fullName – koristi email bez @
      else if (user.email) {
        this.userFullName = user.email.split('@')[0];
      }
      // ✅ 3. Fallback ako baš ništa nema
      else {
        this.userFullName = 'Korisnik';
      }
    }
  }

}
