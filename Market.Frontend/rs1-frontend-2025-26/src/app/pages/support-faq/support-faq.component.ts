import { Component } from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import { AiService } from '../../services/ai.service';


@Component({
  selector: 'app-support-faq',
  imports: [
    CommonModule, // ⬅ za *ngIf, *ngFor
    FormsModule   // ⬅ za ngModel
  ],
  templateUrl: './support-faq.component.html',
  styleUrls: ['./support-faq.component.scss']
})
export class SupportFaqComponent {

  // Lijeva kolona
  faqsLeft = [
    {
      question: 'Kako prijaviti tehnički problem?',
      answer: 'Tehnički problem možete prijaviti putem sekcije Korisnička podrška gdje se nalazi forma za prijavu problema.'
    },
    {
      question: 'Kako mogu kontaktirati korisničku podršku?',
      answer: 'Korisničku podršku možete kontaktirati putem forme ili AI asistenta.'
    },
    {
      question: 'Mogu li izmijeniti ili obrisati poslanu prijavu?',
      answer: 'Trenutno nije moguće mijenjati ili brisati prijave nakon slanja.'
    },
    {
      question: 'Kako funkcioniše sistem bodovanja u aplikaciji?',
      answer: 'Bodovi se dodjeljuju za prijave, volontiranje i druge aktivnosti.'
    },
    {
      question: 'Kako mogu provjeriti status svoje prijave?',
      answer: 'Status prijave možete pratiti u sekciji Moje prijave.'
    }
  ];

  // Desna kolona
  faqsRight = [
    {
      question: 'Da li su prijave tehničkih problema javne?',
      answer: 'Ne. Prijave tehničkih problema su vidljive samo administraciji.'
    },
    {
      question: 'Šta da uradim ako se aplikacija ne učitava?',
      answer: 'Pokušajte osvježiti stranicu ili kontaktirati podršku.'
    },
    {
      question: 'Da li je AI asistent zamjena za korisničku podršku?',
      answer: 'Ne. AI asistent je pomoćni alat, ali ne zamjenjuje podršku.'
    },
    {
      question: 'Da li mogu prijaviti problem bez registracije?',
      answer: 'Ne. Prijava problema zahtijeva prijavljenog korisnika.'
    }
  ];

  openedLeft: number | null = null;
  openedRight: number | null = null;

  toggleLeft(index: number) {
    this.openedLeft = this.openedLeft === index ? null : index;
  }

  toggleRight(index: number) {
    this.openedRight = this.openedRight === index ? null : index;
  }

  // AI
  userQuestion = '';
  aiResponse = '';
  loading = false;
  error = '';

  constructor(private aiService: AiService) {}

  askAi() {
    if (!this.userQuestion.trim()) return;

    this.loading = true;
    this.aiResponse = '';
    this.error = '';

    this.aiService.ask(this.userQuestion).subscribe({
      next: (res) => {
        this.aiResponse = res.answer;
        this.loading = false;
      },
      error: () => {
        this.error = 'Došlo je do greške prilikom poziva AI asistenta.';
        this.loading = false;
      }
    });
  }
}
