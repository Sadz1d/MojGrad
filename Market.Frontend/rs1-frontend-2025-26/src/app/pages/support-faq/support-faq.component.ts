import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-support-faq',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './support-faq.component.html',
  styleUrls: ['./support-faq.component.scss']
})
export class SupportFaqComponent {

  openedIndex: number | null = null;

  faqs = [
    {
      question: 'Kako prijaviti tehnički problem?',
      answer:
        'Tehnički problem možete prijaviti putem sekcije Korisnička podrška, gdje je dostupna forma za prijavu problema.'
    },
    {
      question: 'Da li su prijave tehničkih problema javne?',
      answer:
        'Ne. Prijave tehničkih problema nisu javne i vidljive su isključivo administratorskom timu aplikacije MojGrad.'
    },
    {
      question: 'Kako mogu kontaktirati korisničku podršku?',
      answer:
        'Korisničku podršku možete kontaktirati putem kontakt forme ili putem e-mail adrese navedene u sekciji Podrška.'
    },
    {
      question: 'Šta da uradim ako se aplikacija ne učitava?',
      answer:
        'Provjerite internet konekciju i pokušajte osvježiti stranicu. Ako se problem nastavi, prijavite tehnički problem putem podrške.'
    },
    {
      question: 'Mogu li izmijeniti ili obrisati poslanu prijavu?',
      answer:
        'Nakon slanja, prijavu nije moguće direktno izmijeniti. Za dodatne izmjene potrebno je kontaktirati podršku.'
    },
    {
      question: 'Da li je AI asistent zamjena za korisničku podršku?',
      answer:
        'Ne. AI asistent služi kao pomoć za brze odgovore na najčešća pitanja, dok složenije probleme rješava tim za korisničku podršku.'
    },
    {
      question: 'Kako funkcioniše sistem bodovanja u aplikaciji?',
      answer:
        'Korisnici ostvaruju bodove kroz volontiranje i aktivnosti u aplikaciji, a bodovi se koriste za rang listu i nagrade.'
    },
    {
      question: 'Da li mogu prijaviti problem bez registracije?',
      answer:
        'Ne. Prijava problema je dostupna samo registrovanim korisnicima kako bi se osigurala vjerodostojnost prijava.'
    },
    {
      question: 'Kako mogu provjeriti status svoje prijave?',
      answer:
        'Status prijave možete pratiti u sekciji Moje prijave, gdje su prikazane sve vaše prijavljene aktivnosti.'
    }
  ];


  userQuestion = '';
  aiResponse = '';

  toggle(index: number) {
    this.openedIndex = this.openedIndex === index ? null : index;
  }

  askAi() {
    if (!this.userQuestion.trim()) {
      this.aiResponse = 'Molimo unesite pitanje.';
      return;
    }

    this.aiResponse =
      '🤖 AI asistent: Hvala na pitanju. Ako odgovor nije pronađen u FAQ sekciji, vaš upit će biti proslijeđen podršci.';

    this.userQuestion = '';
  }
}
