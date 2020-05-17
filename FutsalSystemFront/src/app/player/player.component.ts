import { Component, OnInit } from '@angular/core';
import { PlayerService } from '../Services/player.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.scss']
})
export class PlayerComponent implements OnInit {

  public dataSource: any = {};
  public playerid: string;

  constructor(private playerService: PlayerService, private router: Router, private route: ActivatedRoute, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      console.log(params);
      this.playerid = params['id'];
    });

    this.playerService.getPlayer('http://localhost:53078/api/players/' + this.playerid).subscribe(item => {
      this.dataSource = item;
      if(this.dataSource.id === undefined)
      {
        this.router.navigate(['/players']);
      }
    });
  }
}
