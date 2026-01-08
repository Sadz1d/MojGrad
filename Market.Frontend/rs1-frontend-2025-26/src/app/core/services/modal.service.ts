// services/modal.service.ts
import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CommentListComponent } from '../components/comment-list/comment-list.component';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  constructor(private dialog: MatDialog) {}

  openCommentsDialog(reportId: number): void {
    this.dialog.open(CommentListComponent, {
      data: { reportId },
      width: '900px',
      maxWidth: '95vw',
      maxHeight: '90vh',
      panelClass: 'comments-dialog-backdrop',
      disableClose: false,
      autoFocus: false
    });
  }
}