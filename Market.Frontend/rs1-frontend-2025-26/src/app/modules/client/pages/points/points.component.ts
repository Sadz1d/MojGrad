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
      this.userFullName = user.fullName;
    }
  }
}
