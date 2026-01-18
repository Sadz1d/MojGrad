import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-support-faq',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './support-faq.component.html',
  styleUrls: ['./support-faq.component.scss']
})
export class SupportFaqComponent {

  // SVA FAQ PITANJA
  faqs = [
    {
      question: 'Kako prijaviti tehnički problem?',
      answer:
        'Tehnički problem možete prijaviti putem sekcije Korisnička podrška, gdje je dostupna forma za prijavu problema.'
    },
    {
      question: 'Da li su prijave tehničkih problema javne?',
      answer:
        'Ne. Prijave tehničkih problema nisu javne i vidljive su isključivo administratorskom timu.'
    },
    {
      question: 'Kako mogu kontaktirati korisničku podršku?',
      answer:
        'Korisničku podršku možete kontaktirati putem kontakt forme ili e-maila navedenog u sekciji Podrška.'
    },
    {
      question: 'Šta da uradim ako se aplikacija ne učitava?',
      answer:
        'Provjerite internet konekciju i pokušajte osvježiti stranicu. Ako problem i dalje postoji, prijavite tehnički problem.'
    },
    {
      question: 'Mogu li izmijeniti ili obrisati poslanu prijavu?',
      answer:
        'Nakon slanja prijave nije moguće direktno izmijeniti ili obrisati prijavu. Za dodatne izmjene kontaktirajte podršku.'
    },
    {
      question: 'Da li je AI asistent zamjena za korisničku podršku?',
      answer:
        'Ne. AI asistent pomaže u davanju brzih odgovora, ali složenije probleme rješava tim za podršku.'
    },
    {
      question: 'Kako funkcioniše sistem bodovanja u aplikaciji?',
      answer:
        'Korisnici ostvaruju bodove kroz aktivnosti i volontiranje u aplikaciji MojGrad.'
    },
    {
      question: 'Da li mogu prijaviti problem bez registracije?',
      answer:
        'Ne. Prijava problema je dostupna samo registrovanim korisnicima.'
    },
    {
      question: 'Kako mogu provjeriti status svoje prijave?',
      answer:
        'Status svoje prijave možete pratiti u sekciji Moje prijave.'
    }
  ];

  // PODJELA NA LIJEVU I DESNU KOLONU
  faqsLeft = this.faqs.filter((_, i) => i % 2 === 0);
  faqsRight = this.faqs.filter((_, i) => i % 2 === 1);

  // OTVORENA PITANJA
  openedLeft: number | null = null;
  openedRight: number | null = null;

  // TOGGLE FUNKCIJE
  toggleLeft(index: number) {
    this.openedLeft = this.openedLeft === index ? null : index;
  }

  toggleRight(index: number) {
    this.openedRight = this.openedRight === index ? null : index;
  }

  // AI DIO
  userQuestion = '';
  aiResponse = '';

  askAi() {
    if (!this.userQuestion.trim()) {
      this.aiResponse = 'Molimo unesite pitanje.';
      return;
    }

    this.aiResponse =
      '🤖 AI asistent: Hvala na pitanju. Ako odgovor nije pronađen u FAQ sekciji, vaš upit je proslijeđen podršci.';

    this.userQuestion = '';
  }
}
