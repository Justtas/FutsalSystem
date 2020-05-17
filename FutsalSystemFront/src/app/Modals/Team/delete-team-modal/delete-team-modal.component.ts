import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-delete-team-modal',
  templateUrl: './delete-team-modal.component.html',
  styleUrls: ['./delete-team-modal.component.scss']
})
export class DeleteTeamModalComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<DeleteTeamModalComponent>,
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
