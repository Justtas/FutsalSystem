import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AnnouncementDto } from 'src/app/Models/AnnouncementDto';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AnnouncementService } from 'src/app/Services/announcement.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-create-announcement-modal',
  templateUrl: './create-announcement-modal.component.html',
  styleUrls: ['./create-announcement-modal.component.scss']
})
export class CreateAnnouncementModalComponent implements OnInit {

  public announcement: AnnouncementDto;

  bioSection = new FormGroup({
    Title: new FormGroup({
      title: new FormControl('')
    }),
    Content: new FormGroup({
      content: new FormControl('')
    })
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private announcementService: AnnouncementService, private router: Router, private snackBar: MatSnackBar, public dialogRef: MatDialogRef<CreateAnnouncementModalComponent>) { }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  updateAnnouncementData() {
    if (this.validated())
    {
      this.announcement = new AnnouncementDto(0, this.bioSection.value.Title.title, "", this.bioSection.value.Content.content);
      console.log(this.announcement);
      this.announcementService.postAnnouncement('http://localhost:53078/api/announcements/', this.announcement)
        .subscribe(anything => { console.log(anything); });
      this.onNoClick();
    } else {
      this.snackBar.open("Duomenų laukai užpildyti neteisingai.", "Gerai", {
        duration: 4000,
      });
    }
  }

  openCreateSuccessfulBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }

  validated(): boolean {
    if (
      this.bioSection.value.Title.title === '' ||
      this.bioSection.value.Title.title === undefined ||
      this.bioSection.value.Content.content === '' ||
      this.bioSection.value.Content.content === undefined) {
      return false;
    }
    return true;
  }

}
