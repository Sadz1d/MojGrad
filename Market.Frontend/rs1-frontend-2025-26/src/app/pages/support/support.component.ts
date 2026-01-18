import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-support',
  standalone: true,
  imports: [],
  templateUrl: './support.component.html',
  styleUrls: ['./support.component.scss']
})

export class SupportComponent {

  constructor(private router: Router) {}

  sendEmail() {
    const email = 'podrska@mojgrad.ba';
    const subject = 'Podrška – MojGrad';
    const body = 'Poštovani,%0D%0A%0D%0AImam pitanje / problem u vezi aplikacije MojGrad.%0D%0A%0D%0AHvala.';

    const gmailUrl =
      `https://mail.google.com/mail/?view=cm&fs=1&to=${email}&su=${subject}&body=${body}`;

    window.open(gmailUrl, '_blank');
  }


  reportProblem() {
    this.router.navigate(['/support/tech-issue']);
  }

  openFaq() {
    this.router.navigate(['/support/faq']);
  }

}

