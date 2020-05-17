import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-delete-player-modal',
  templateUrl: './delete-player-modal.component.html',
  styleUrls: ['./delete-player-modal.component.scss']
})
export class DeletePlayerModalComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<DeletePlayerModalComponent>,
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
