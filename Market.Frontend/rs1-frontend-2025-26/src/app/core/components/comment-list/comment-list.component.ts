import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatDivider } from '@angular/material/divider';
import { MatFormField, MatLabel, MatError, MatHint } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CommentService } from '../../services/comment.service';
import { ProblemReportService } from '../../services/problem-report.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';
import { CommentListItem, CreateCommentCommand } from '../../models/comment.model';
import { ProblemReportDetail } from '../../models/problem-report.model';

@Component({
  selector: 'app-comment-list',
  templateUrl: './comment-list.component.html',
  styleUrls: ['./comment-list.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatIcon,
    MatProgressSpinner,

    MatFormField,
    MatInput,
    MatButton,
    MatLabel,
    MatError,
    MatHint
  ]
})
export class CommentListComponent implements OnInit {
  comments: CommentListItem[] = [];
  report: ProblemReportDetail | null = null;
  loading = false;
  loadingReport = false;
  error = '';
  submittingComment = false;

  // Trenutni korisnik
  currentUserId: number | null = null;
  currentUserRole: string = 'user';
  isAdmin = false;
  isManager = false;
  isAuthenticated = false;

  // Paginacija
  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  totalPages = 0;

  // Forma za novi komentar
  commentForm: FormGroup;

  constructor(
    private commentService: CommentService,
    private problemReportService: ProblemReportService,
    private currentUserService: CurrentUserService,
    private router: Router,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CommentListComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { reportId: number }
  ) {
    this.commentForm = this.fb.group({
      text: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(2000)]]
    });
  }

  ngOnInit(): void {
    this.loadCurrentUser();
    this.loadReportDetails();
    this.loadComments();
  }

  private loadCurrentUser(): void {
    const user = this.currentUserService.snapshot;
    this.isAuthenticated = this.currentUserService.isAuthenticated();
    this.isAdmin = this.currentUserService.isAdmin();
    this.isManager = this.currentUserService.isManager();

    if (user) {
      // Pretvori string ID u number
      this.currentUserId = this.currentUserId = user.id || null;

      // Odredi rolu
      if (this.isAdmin) this.currentUserRole = 'admin';
      else if (this.isManager) this.currentUserRole = 'manager';
      else if (this.currentUserService.isEmployee()) this.currentUserRole = 'employee';
      else this.currentUserRole = 'user';
    }
  }

  loadReportDetails(): void {
    this.loadingReport = true;
    this.problemReportService.getReport(this.data.reportId).subscribe({
      next: (report) => {
        this.report = report;
        this.loadingReport = false;
      },
      error: (err) => {
        this.error = 'Greška pri učitavanju detalja prijave: ' + err.message;
        this.loadingReport = false;
      }
    });
  }

  loadComments(): void {
    this.loading = true;
    this.error = '';

    this.commentService.getComments({
      reportId: this.data.reportId,
      page: this.currentPage,
      pageSize: this.pageSize,
      sortBy: 'publicationDate',
      sortDirection: 'desc'
    }).subscribe({
      next: (response) => {
        this.comments = response.items;
        this.totalItems = response.totalCount;
        this.totalPages = response.totalPages;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Greška pri učitavanju komentara: ' + err.message;
        this.loading = false;
      }
    });
  }

  onSubmitComment(): void {
    if (!this.isAuthenticated) {
      this.error = 'Morate biti prijavljeni da biste ostavili komentar';
      return;
    }

    if (this.commentForm.invalid) return;

    this.submittingComment = true;

    const command: CreateCommentCommand = {
      reportId: this.data.reportId,
      userId: this.currentUserId!,
      text: this.commentForm.get('text')?.value
    };

    this.commentService.createComment(command).subscribe({
      next: (commentId) => {
        // Resetuj formu
        this.commentForm.reset();
        this.commentForm.markAsPristine();
        this.commentForm.markAsUntouched();

        // Ponovo učitaj komentare
        this.loadComments();
        this.submittingComment = false;
      },
      error: (err) => {
        this.error = 'Greška pri slanju komentara: ' + err.message;
        this.submittingComment = false;
      }
    });
  }

  editComment(comment: CommentListItem): void {
    if (!this.canEditComment(comment)) return;

    const newText = prompt('Uredite komentar:', comment.text);
    if (newText && newText.trim() !== comment.text) {
      this.commentService.updateComment(comment.id, {
        id: comment.id,
        text: newText.trim()
      }).subscribe({
        next: () => {
          this.loadComments(); // Refresh list
        },
        error: (err) => {
          alert('Greška pri ažuriranju komentara: ' + err.message);
        }
      });
    }
  }

  deleteComment(commentId: number): void {
    if (confirm('Da li ste sigurni da želite da obrišete ovaj komentar?')) {
      this.commentService.deleteComment(commentId).subscribe({
        next: () => {
          this.comments = this.comments.filter(c => c.id !== commentId);
          this.totalItems--;
        },
        error: (err) => {
          alert('Greška pri brisanju komentara: ' + err.message);
        }
      });
    }
  }

  canEditComment(comment: CommentListItem): boolean {
    if (!this.isAuthenticated) return false;

    // Vlasnik komentara može da edituje
    if (this.currentUserId === comment.userId) return true;

    // Admin ili menadžer može da edituje
    if (this.isAdmin || this.isManager) return true;

    return false;
  }

  canDeleteComment(comment: CommentListItem): boolean {
    return this.canEditComment(comment); // Ista pravila za brisanje
  }

  onPageChange(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.loadComments();
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisiblePages = 5;

    let startPage = Math.max(1, this.currentPage - Math.floor(maxVisiblePages / 2));
    let endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);

    if (endPage - startPage + 1 < maxVisiblePages) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('bs-BA', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  goBackToReports(): void {
    this.closeDialog();
    this.router.navigate(['/problem-reports']);
  }

  goToLogin(): void {
    this.closeDialog();
    this.router.navigate(['/auth/login'], {
      queryParams: { returnUrl: `/problem-reports/${this.data.reportId}` }
    });
  }


// Mock metoda - trebalo bi da dobijete podatke sa servera
public isAdminUser(userId: number): boolean {
  // Ovdje treba da proverite da li je korisnik admin
  // Za sada ćemo koristiti trenutnog korisnika
  return this.isAdmin && this.currentUserId === userId;
}

public isManagerUser(userId: number): boolean {
  // Ovdje treba da proverite da li je korisnik menadžer
  return this.isManager && this.currentUserId === userId;
}

public getUserRole(userId: number): string {
  // Ovo bi trebalo da dobijete sa servera
  // Za sada vraćamo rolu trenutnog korisnika
  if (userId === this.currentUserId) {
    return this.currentUserRole;
  }

  // Ako nije trenutni korisnik, možete implementirati cache ili API call
  return 'user';
}
}
