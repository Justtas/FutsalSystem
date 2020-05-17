import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AnnouncementService } from 'src/app/Services/announcement.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-edit-announcement-modal',
  templateUrl: './edit-announcement-modal.component.html',
  styleUrls: ['./edit-announcement-modal.component.scss']
})
export class EditAnnouncementModalComponent implements OnInit {

  public tempObject: any;

  bioSection = new FormGroup({
    Title: new FormGroup({
      title: new FormControl(this.data.title)
    }),
    Message: new FormGroup({
      message: new FormControl(this.data.message)
    })
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private announcementService: AnnouncementService, private snackBar: MatSnackBar, public dialogRef: MatDialogRef<EditAnnouncementModalComponent>) { this.tempObject = data; }

  ngOnInit(): void {
    console.log(this.data);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  updateAnnouncementData() {
    if (this.validated())
    {
      this.tempObject.title = this.bioSection.value.Title.title;
      this.tempObject.message = this.bioSection.value.Message.message;
      this.announcementService.UpdateAnnouncement('http://localhost:53078/api/announcements/' + this.tempObject.id, this.tempObject)
        .subscribe(anything => { });
      this.onNoClick();
    } else {
      this.snackBar.open("Duomenų laukai užpildyti neteisingai.", "Gerai", {
        duration: 4000,
      });
    }
  }

  openEditSuccessfulBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }

  validated(): boolean {
    if (
      this.bioSection.value.Title.title === '' ||
      this.bioSection.value.Title.title === undefined ||
      this.bioSection.value.Message.message === '' ||
      this.bioSection.value.Message.message === undefined) {
      return false;
    }
    return true;
  }

}
