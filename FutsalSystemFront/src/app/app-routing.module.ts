import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { TeamsComponent } from './teams/teams.component';
import { PlayersComponent } from './players/players.component';
import { TeamComponent } from './team/team.component';
import { PlayerComponent } from './player/player.component';
import { MatchesComponent } from './matches/matches.component';
import { MatchComponent } from './match/match.component';
import { StatisticsComponent } from './statistics/statistics.component';


const routes: Routes = [
  {path:'home', component: HomeComponent},
  {path:'login', component: LoginComponent},
  {path:'teams', component: TeamsComponent},
  {path:'teams/:id', component: TeamComponent},
  {path:'teams/:id/players', component: PlayersComponent},
  {path:'players/:id', component: PlayerComponent},
  {path:'players', component: PlayersComponent},
  {path:'matches', component: MatchesComponent},
  {path:'matches/:id', component: MatchComponent},
  {path:'statistics', component: StatisticsComponent},
  {path:'**', redirectTo: '/home'},
  {path: '', redirectTo: '/home', pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
