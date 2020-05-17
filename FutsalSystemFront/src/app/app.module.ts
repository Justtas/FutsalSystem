import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'

//Angular Material Components
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from '@angular/material/input';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatRadioModule} from '@angular/material/radio';
import {MatSelectModule} from '@angular/material/select';
import {MatSliderModule} from '@angular/material/slider';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import {MatMenuModule} from '@angular/material/menu';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatListModule} from '@angular/material/list';
import {MatGridListModule} from '@angular/material/grid-list';
import {MatCardModule} from '@angular/material/card';
import {MatStepperModule} from '@angular/material/stepper';
import {MatTabsModule} from '@angular/material/tabs';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatButtonToggleModule} from '@angular/material/button-toggle';
import {MatChipsModule} from '@angular/material/chips';
import {MatIconModule} from '@angular/material/icon';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import {MatDialogModule} from '@angular/material/dialog';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {MatTableModule} from '@angular/material/table';
import {MatSortModule} from '@angular/material/sort';
import {MatPaginatorModule} from '@angular/material/paginator';

import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { DatePipe } from '@angular/common';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { NavbarComponent } from './navbar/navbar.component';
import { TeamsComponent } from './teams/teams.component';
import { PlayersComponent } from './players/players.component';
import { PlayerComponent } from './player/player.component';
import { TeamComponent } from './team/team.component';
import { AnnouncementsComponent } from './announcements/announcements.component';
import { MatchesComponent } from './matches/matches.component';
import { MatchComponent } from './match/match.component';

import { EditTeamModalComponent } from './Modals/Team/edit-team-modal/edit-team-modal.component';
import { CreatePlayerModalComponent } from './Modals/Player/create-player-modal/create-player-modal.component';
import { EditPlayerModalComponent } from './Modals/Player/edit-player-modal/edit-player-modal.component';
import { CreateTeamComponent } from './Modals/Team/create-team-modal/create-team-modal.component';
import { DeleteTeamModalComponent } from './Modals/Team/delete-team-modal/delete-team-modal.component';
import { CreateAnnouncementModalComponent } from './Modals/Announcement/create-announcement-modal/create-announcement-modal.component';
import { EditAnnouncementModalComponent } from './Modals/Announcement/edit-announcement-modal/edit-announcement-modal.component';
import { DeleteAnnouncementModalComponent } from './Modals/Announcement/delete-announcement-modal/delete-announcement-modal.component';
import { StatisticsComponent } from './statistics/statistics.component';
import { DeletePlayerModalComponent } from './Modals/Player/delete-player-modal/delete-player-modal.component';
import { CreateMatchModalComponent } from './Modals/Match/create-match-modal/create-match-modal.component';
import { EditMatchModalComponent } from './Modals/Match/edit-match-modal/edit-match-modal.component';
import { DeleteMatchModalComponent } from './Modals/Match/delete-match-modal/delete-match-modal.component';
import { CreateMatchEventModalComponent } from './Modals/Match/create-match-event-modal/create-match-event-modal.component';
import { DeleteMatchEventModalComponent } from './Modals/Match/delete-match-event-modal/delete-match-event-modal.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    NavbarComponent,
    TeamsComponent,
    PlayersComponent,
    PlayerComponent,
    TeamComponent,
    AnnouncementsComponent,
    MatchesComponent,
    MatchComponent,
    CreateTeamComponent,
    DeleteTeamModalComponent,
    EditTeamModalComponent,
    CreatePlayerModalComponent,
    EditPlayerModalComponent,
    CreateAnnouncementModalComponent,
    EditAnnouncementModalComponent,
    DeleteAnnouncementModalComponent,
    StatisticsComponent,
    DeletePlayerModalComponent,
    CreateMatchModalComponent,
    EditMatchModalComponent,
    DeleteMatchModalComponent,
    CreateMatchEventModalComponent,
    DeleteMatchEventModalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatCheckboxModule,
    MatCheckboxModule,
    MatButtonModule,
    MatInputModule,
    MatAutocompleteModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatRadioModule,
    MatSelectModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatMenuModule,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatGridListModule,
    MatCardModule,
    MatStepperModule,
    MatTabsModule,
    MatExpansionModule,
    MatButtonToggleModule,
    MatChipsModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatDialogModule,
    MatTooltipModule,
    MatSnackBarModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    FlexLayoutModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
