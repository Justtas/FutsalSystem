import { Component, OnInit } from '@angular/core';
import { PlayerService } from '../Services/player.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CreatePlayerModalComponent } from '../Modals/Player/create-player-modal/create-player-modal.component';
import { TeamDto } from '../Models/TeamDto';
import { TeamService } from '../Services/team.service';
import { EditPlayerModalComponent } from '../Modals/Player/edit-player-modal/edit-player-modal.component';
import { DeletePlayerModalComponent } from '../Modals/Player/delete-player-modal/delete-player-modal.component';
import { HubService } from '../Services/hub.service';

@Component({
  selector: 'app-players',
  templateUrl: './players.component.html',
  styleUrls: ['./players.component.scss']
})

export class PlayersComponent implements OnInit {

  public dataSource: any[] = [];
  public teamsDto: any[];
  public fullInfo: any[] = [];
  public fullInfoCoppied: any[] = [];
  public searchTeam: any;
  public isAdmin = false;
  public notLoaded = true;

  constructor(private playerService: PlayerService, private teamService: TeamService, private router: Router, private route: ActivatedRoute, public dialog: MatDialog, private signalRService: HubService) { }

  ngOnInit(): void {
    //localStorage.setItem('isAdmin', 'true');
    //this.isAdmin = Boolean(localStorage.getItem('isAdmin'));
    if (localStorage.getItem("userId") !== null) {
      this.isAdmin = true;
    }

    this.signalRService.playerCreationSignal.subscribe((signal: any) => {
      this.onPlayerCreated(signal);
    });

    this.signalRService.playerUpdateSignal.subscribe((signal: any) => {
      this.onPlayerUpdated(signal);
    });

    //this.playerService.getPlayers('http://localhost:53078/api/players/byteam/' + this.teamid).subscribe(items => { this.dataSource = items; console.log(this.dataSource); });
    this.playerService.getPlayers('http://localhost:53078/api/players').subscribe(x => {
      this.dataSource = x;
      this.teamService.getTeams('http://localhost:53078/api/teams/').subscribe(y => {
        this.teamsDto = y;
        this.dataSource.forEach(player => {
          if (player.teamId === null) {
            let temp = {
              id: player.id,
              name: player.firstName,
              lastname: player.lastName,
              dateOfBirth: player.dateOfBirth,
              imagePath: player.imagePath,
              number: player.number,
              teamId: null,
              teamName: 'NĖRA'
            }
            this.fullInfo.push(temp);
          }
          this.teamsDto.forEach(team => {
            if (player.teamId === team.id) {
              let temp = {
                id: player.id,
                name: player.firstName,
                lastname: player.lastName,
                dateOfBirth: player.dateOfBirth,
                imagePath: player.imagePath,
                number: player.number,
                teamId: player.teamId,
                teamName: team.title
              }
              this.fullInfo.push(temp);
            }
          });
        });
        this.assignCopy();
      });
      this.notLoaded = false;
    });
  }

  openPlayerCreateDialog() {
    let dialogRef = this.dialog.open(CreatePlayerModalComponent);

    dialogRef.afterClosed().subscribe(result => {
      // let temp = [];
      // this.playerService.getPlayers('http://localhost:53078/api/players/').subscribe(items => {
      //   temp = items;
      //   if (temp.length != this.dataSource.length)
      //     this.dataSource.push(temp[temp.length - 1]);
      // });
    });
  }

  onPlayerRowClick(player: any) {
    this.router.navigate(['/players', player.id]);
  }

  assignCopy() {
    this.fullInfoCoppied = Object.assign([], this.fullInfo);
  }

  filterItem(value) {
    if (!value) {
      this.assignCopy();
    } // when nothing has typed
    this.fullInfoCoppied = Object.assign([], this.fullInfo).filter(
      item => item.teamName.toLowerCase().indexOf(value.toLowerCase()) > -1 ||
        item.name.toLowerCase().indexOf(value.toLowerCase()) > -1 ||
        item.lastname.toLowerCase().indexOf(value.toLowerCase()) > -1 ||
        item.number.toString().toLowerCase().indexOf(value.toString().toLowerCase()) > -1
    )
  }

  openPlayerDeleteDialog(item, index, event): void {
    event.stopPropagation();
    const dialogRef = this.dialog.open(DeletePlayerModalComponent, {
      width: '350px',
      data: "Ar tikrai norite ištrinti šį žaidėją?"
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        let temp = [];
        this.playerService.deletePlayer('http://localhost:53078/api/players/' + item.id).subscribe(item => {
          this.fullInfo.splice(index, 1);
          this.assignCopy();
        });
      }
    });
  }

  openPlayerEditDialog(player: any, event) {
    event.stopPropagation();
    console.log(player);
    let dialogRef = this.dialog.open(EditPlayerModalComponent, { data: player });

    dialogRef.afterClosed().subscribe(result => {
      console.log('Dialog result:' + result);

    });
  }

  onPlayerCreated(signal) {

    let teamName = 0;
    this.teamsDto.forEach(element => {
      if (element.id === signal.teamId) {
        teamName = element.title;
      }
    });

    let temp = {
      dateOfBirth: signal.dateOfBirth,
      id: signal.id,
      imagePath: signal.imagePath,
      lastname: signal.lastName,
      name: signal.firstName,
      number: signal.number,
      teamName: teamName
    };
    this.fullInfo.push(temp);
    this.assignCopy();
  }

  onPlayerUpdated(signal) {
    console.log(signal);
    let teamName = 0;
    this.teamsDto.forEach(element => {
      if (element.id === signal.teamId) {
        teamName = signal.title;
      }
    });

    this.fullInfo.forEach(element => {
      if (element.id === signal.id) {
        element.dateOfBirth = signal.dateOfBirth,
          element.imagePath = signal.imagePath,
          element.lastname = signal.lastName,
          element.name = signal.firstName,
          element.number = signal.number,
          element.teamId = signal.teamId,
          element.teamName = teamName;
      }
    });
    this.assignCopy();
  }
}
