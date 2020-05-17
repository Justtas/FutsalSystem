import { Component, OnInit, Inject, Pipe, PipeTransform } from '@angular/core';
import { PlayerDto } from 'src/app/Models/PlayerDto';
import { PlayerService } from 'src/app/Services/player.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormGroup, FormControl } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatchEventDto } from 'src/app/Models/MatchEventDto';
import { MatchEventService } from 'src/app/Services/matchEvent.service';
import { PlayerEvent } from 'src/app/Enums/PlayerEvent';

@Pipe({ name: 'enumToArray' })
export class EnumToArrayPipe implements PipeTransform {
  transform(value): Object {
    return Object.keys(value).filter(e => !isNaN(+e)).map(o => { return { index: +o, name: value[o] } });
  }
}

@Component({
  selector: 'app-create-match-event-modal',
  templateUrl: './create-match-event-modal.component.html',
  styleUrls: ['./create-match-event-modal.component.scss']
})
export class CreateMatchEventModalComponent implements OnInit {

  public teamPlayers: PlayerDto[] = [];
  public teamAPlayers: PlayerDto[] = [];
  public teamBPlayers: PlayerDto[] = [];
  public matchObject: any = {};
  public selectedPlayer: any = {};
  //selectedPlayerId = -1;
  public selectedMatchId: string;
  public temp: any = {};

  public createdMatchEvents: MatchEventDto[] = [];

  public matchEvent: MatchEventDto;
  public selectedEvent: PlayerEvent;
  public matchEvents = [];

  public keys: any;

  bioSection = new FormGroup({
    Minute: new FormGroup({
      minute: new FormControl('')
    })
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private playerService: PlayerService, private matchEventService: MatchEventService, private router: Router, private route: ActivatedRoute, public dialog: MatDialog, private snackBar: MatSnackBar, public dialogRef: MatDialogRef<CreateMatchEventModalComponent>) {
     this.matchObject = data;
  }

  onNoClick(element: any): void {
    this.dialogRef.close(element);
  }

  ngOnInit(): void {
    this.selectedMatchId = this.data.id;
    this.route.params.subscribe(params => {
    });
    console.log(this.data);
    console.log(this.selectedEvent);
    this.playerService.getPlayers('http://localhost:53078/api/players/byTeam/' + this.data.homeTeamId).subscribe(items => {
      this.teamAPlayers = items;
      console.log(this.teamAPlayers);
      this.playerService.getPlayers('http://localhost:53078/api/players/byTeam/' + this.data.awayTeamId).subscribe(items => {
        this.teamBPlayers = items;
        console.log(this.teamBPlayers);
        this.teamPlayers = this.teamAPlayers.concat(this.teamBPlayers);
      });
    });
  }

  openCreateSuccessfulBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 2000,
    });
  }

  updateMatchEventData() {
    if (this.validated()) {
      console.log(this.selectedPlayer);
      let playerName = `${this.selectedPlayer.firstName} ${this.selectedPlayer.lastName}`;
      this.temp = {
        id: 0,
        matchId: this.data.id,
        minute: this.bioSection.value.Minute.minute,
        playerId: this.selectedPlayer.id,
        teamName: this.selectedPlayer.teamName,
        playerName: playerName,
        eventType: this.selectedEvent
      }
      console.log(this.temp);
      this.onNoClick(this.temp);
    } else {
      this.snackBar.open("Duomenų laukai užpildyti neteisingai.", "Supratau", {
        duration: 4000,
      });
    }
  }

  validated(): boolean {

    if (
      this.bioSection.value.Minute.minute === '' ||
      this.bioSection.value.Minute.minute === undefined ||
      this.bioSection.value.Minute.minute <= 0 ||
      this.bioSection.value.Minute.minute > 40 ||
      this.selectedPlayer.teamName === undefined ||
      this.selectedPlayer.Id === -1 ||
      this.selectedEvent == undefined) {
      return false;
    }
    return true;
  }

}
