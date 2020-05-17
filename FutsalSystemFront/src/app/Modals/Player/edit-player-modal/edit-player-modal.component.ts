import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { TeamDto } from 'src/app/Models/TeamDto';
import { PlayerDto } from 'src/app/Models/PlayerDto';
import { PlayerService } from 'src/app/Services/player.service';
import { TeamService } from 'src/app/Services/team.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-edit-player-modal',
  templateUrl: './edit-player-modal.component.html',
  styleUrls: ['./edit-player-modal.component.scss']
})
export class EditPlayerModalComponent implements OnInit {

  public tempObject: PlayerDto;
  public teams: TeamDto[];
  selectedTeamId: number = this.data.teamId;
  public imageBase64Code: string = "";
  public newDateOfBirth = new Date(this.data.dateOfBirth);

  bioSection = new FormGroup({
    FirstName: new FormGroup({
      firstName: new FormControl(this.data.name)
    }),
    LastName: new FormGroup({
      lastName: new FormControl(this.data.lastname)
    }),
    DateOfBirth: new FormGroup({
      dateOfBirth: new FormControl(this.datepipe.transform(this.data.dateOfBirth, 'yyyy-MM-dd'))
    }),
    Number: new FormGroup({
      number: new FormControl(this.data.number)
    })
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private playerService: PlayerService, private teamService: TeamService, private router: Router, private snackBar: MatSnackBar, public datepipe: DatePipe, public dialogRef: MatDialogRef<EditPlayerModalComponent>) { this.tempObject = data; }

  ngOnInit(): void {
    this.teamService.getTeams('http://localhost:53078/api/teams/').subscribe(items => { this.teams = items; console.log(this.data); });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  updatePlayerData() {
    if (this.validated()) {
      this.tempObject.Id = this.data.id;
      this.tempObject.FirstName = this.bioSection.value.FirstName.firstName;
      this.tempObject.LastName = this.bioSection.value.LastName.lastName;
      this.tempObject.DateOfBirth = this.bioSection.value.DateOfBirth.dateOfBirth;
      this.tempObject.Number = this.bioSection.value.Number.number;
      this.tempObject.TeamId = this.selectedTeamId;
      this.tempObject.ImagePath = this.data.imagePath;
      console.log(this.tempObject);
      if (this.imageBase64Code != "") {
        this.tempObject.ImagePath = this.imageBase64Code;
      }
      this.playerService.UpdatePlayer('http://localhost:53078/api/players/' + this.tempObject.Id, this.tempObject)
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
    let dateLimitMin = new Date('1950-01-01').toISOString();
    let dateLimitMax = new Date('2010-01-01').toISOString();
    if (
      this.bioSection.value.FirstName.firstName === undefined ||
      this.bioSection.value.LastName.lastName === undefined ||
      this.bioSection.value.FirstName.firstName === '' ||
      this.bioSection.value.LastName.lastName === '' ||
      this.bioSection.value.Number.number === '' ||
      this.bioSection.value.Number.number < 0 ||
      this.bioSection.value.Number.number > 99 ||
      this.selectedTeamId === undefined ||
      this.bioSection.value.DateOfBirth.dateOfBirth < dateLimitMin ||
      this.bioSection.value.DateOfBirth.dateOfBirth > dateLimitMax) {
      return false;
    }
    return true;
  }

}
