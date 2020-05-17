import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TeamService } from 'src/app/Services/team.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-edit-team-modal',
  templateUrl: './edit-team-modal.component.html',
  styleUrls: ['./edit-team-modal.component.scss']
})
export class EditTeamModalComponent implements OnInit {

  public tempObject: any;
  public imageBase64Code: string = "";


  bioSection = new FormGroup({
    Title: new FormGroup({
      title: new FormControl(this.data.title)
    })
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private teamService: TeamService, private snackBar: MatSnackBar, public dialogRef: MatDialogRef<EditTeamModalComponent>) { this.tempObject = data; }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  updateTeamData() {
    if (this.validated())
    {
      this.tempObject.title = this.bioSection.value.Title.title;
      if (this.imageBase64Code != "") {
        this.tempObject.imagePath = this.imageBase64Code;
      }
      this.teamService.UpdateTeam('http://localhost:53078/api/teams/' + this.tempObject.id, this.tempObject)
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

  handleFileSelect(evt) {
    var files = evt.target.files;
    var file = files[0];

    if (files && file) {
      var reader = new FileReader();

      reader.onload = this._handleReaderLoaded.bind(this);

      reader.readAsBinaryString(file);
    }
  }

  _handleReaderLoaded(readerEvt) {
    var binaryString = readerEvt.target.result;
    this.imageBase64Code = btoa(binaryString);
  }

  validated(): boolean {
    if (
      this.bioSection.value.Title.title === '' ||
      this.bioSection.value.Title.title === undefined) {
      return false;
    }
    return true;
  }
}
