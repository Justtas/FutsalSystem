import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TeamDto } from 'src/app/Models/TeamDto';
import { TeamService } from 'src/app/Services/team.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-create-team',
  templateUrl: './create-team-modal.component.html',
  styleUrls: ['./create-team-modal.component.scss']
})
export class CreateTeamComponent implements OnInit {

  public temp: TeamDto;
  public imageBase64Code: string = "";

  bioSection = new FormGroup({
    Title: new FormGroup({
      title: new FormControl('')
    })
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private teamService: TeamService, private router: Router, private snackBar: MatSnackBar, public dialogRef: MatDialogRef<CreateTeamComponent>) { }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  updateTeamData() {
    if (this.validated())
    {
      this.temp = new TeamDto(0, this.bioSection.value.Title.title, 0, 0, 0, 0, 0, 0, 0, this.imageBase64Code, [], [], [], 0);
      console.log(this.temp);
      this.teamService.postTeam('http://localhost:53078/api/teams/', this.temp)
        .subscribe(anything => { console.log(anything) });
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

  handleFileSelect(evt) {
    var files = evt.target.files;
    var file = files[0];
    var fileType = file['type'];
    var validImageTypes = ['image/jpeg', 'image/jpg', 'image/png'];

    if (files && file && validImageTypes.includes(fileType)) {
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
