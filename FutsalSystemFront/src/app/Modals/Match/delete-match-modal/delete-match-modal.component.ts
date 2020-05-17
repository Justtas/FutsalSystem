import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-delete-match-modal',
  templateUrl: './delete-match-modal.component.html',
  styleUrls: ['./delete-match-modal.component.scss']
})
export class DeleteMatchModalComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<DeleteMatchModalComponent>,
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
