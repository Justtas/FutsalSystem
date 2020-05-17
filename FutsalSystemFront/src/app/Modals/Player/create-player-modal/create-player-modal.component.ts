import { Component, OnInit, Inject } from '@angular/core';
import { PlayerService } from 'src/app/Services/player.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PlayerDto } from 'src/app/Models/PlayerDto';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { TeamService } from 'src/app/Services/team.service';
import { TeamDto } from 'src/app/Models/TeamDto';

@Component({
  selector: 'app-create-player-modal',
  templateUrl: './create-player-modal.component.html',
  styleUrls: ['./create-player-modal.component.scss']
})
export class CreatePlayerModalComponent implements OnInit {



  public temp: PlayerDto;
  public teams: TeamDto[];
  selectedTeamId: number;
  selectedTeam: any;
  public imageBase64Code: string = "";
  selectedTeamName: string;

  bioSection = new FormGroup({
    FirstName: new FormGroup({
      firstName: new FormControl('')
    }),
    LastName: new FormGroup({
      lastName: new FormControl('')
    }),
    DateOfBirth: new FormGroup({
      dateOfBirth: new FormControl('')
    }),
    Number: new FormGroup({
      number: new FormControl('')
    })
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private playerService: PlayerService, private teamService: TeamService, private router: Router, private snackBar: MatSnackBar, public dialogRef: MatDialogRef<CreatePlayerModalComponent>) { }

  ngOnInit(): void {
    this.teamService.getTeams('http://localhost:53078/api/teams/').subscribe(items => { this.teams = items; });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  updatePlayerData() {
    if (this.validated()) {
      console.log(this.selectedTeam);
      this.temp = new PlayerDto(0, this.bioSection.value.FirstName.firstName, this.bioSection.value.LastName.lastName, this.bioSection.value.DateOfBirth.dateOfBirth, this.imageBase64Code, 0, 0, 0, 0, 0, this.bioSection.value.Number.number, this.selectedTeam.id, this.selectedTeam.title);
      this.playerService.postPlayer('http://localhost:53078/api/players/', this.temp)
        .subscribe(anything => { });
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
      this.selectedTeam.id === undefined ||
      this.bioSection.value.DateOfBirth.dateOfBirth < dateLimitMin ||
      this.bioSection.value.DateOfBirth.dateOfBirth > dateLimitMax ) {
      return false;
    }
    return true;
  }
}
