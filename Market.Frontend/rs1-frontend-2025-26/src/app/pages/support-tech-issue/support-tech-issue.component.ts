import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-support-tech-issue',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './support-tech-issue.component.html',
  styleUrls: ['./support-tech-issue.component.scss']
})
export class SupportTechIssueComponent {

  type = 'Bug';
  description = '';

  send() {
    const email = 'podrska@mojgrad.ba';
    const subject = encodeURIComponent('Tehnički problem – MojGrad');
    const body = encodeURIComponent(
      `Vrsta problema: ${this.type}\n\nOpis problema:\n${this.description}`
    );

    const gmailUrl =
      `https://mail.google.com/mail/?view=cm&fs=1&to=${email}&su=${subject}&body=${body}`;

    window.open(gmailUrl, '_blank');
  }
}

