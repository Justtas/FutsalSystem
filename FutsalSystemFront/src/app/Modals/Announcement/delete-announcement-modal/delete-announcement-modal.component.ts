import { Component, OnInit, Inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-delete-announcement-modal',
  templateUrl: './delete-announcement-modal.component.html',
  styleUrls: ['./delete-announcement-modal.component.scss']
})
export class DeleteAnnouncementModalComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<DeleteAnnouncementModalComponent>,
    @Inject(MAT_DIALOG_DATA) public message: string, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  openDeleteSuccessfulBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }

}
