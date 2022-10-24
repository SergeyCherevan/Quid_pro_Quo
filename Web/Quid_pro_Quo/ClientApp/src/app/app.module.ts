import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';
import { GoogleMapsModule } from '@angular/google-maps';

import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { TimelinePageComponent } from './components/timeline-page/timeline-page.component';
import { SearchGeomarkerFormComponent } from './components/search-geomarker-form/search-geomarker-form.component';
import { PostComponent } from './components/post/post.component';
import { PostPageComponent } from './components/post-page/post-page.component';
import { AddPostPageComponent } from './components/add-post-page/add-post-page.component';
import { UsersPageComponent } from './components/users-page/users-page.component';
import { UserPageComponent } from './components/user-page/user-page.component';
import { MessengerPageComponent } from './components/messenger-components/messenger-page/messenger-page.component';
import { AuthorizationFormComponent } from './components/authorization-form/authorization-form.component';
import { AccountPageComponent } from './components/account-page/account-page.component';
import { NotFoundPageComponent } from './components/not-found-page/not-found-page.component';
import { EditPostFormComponent } from './components/edit-post-form/edit-post-form.component';
import { ChooseMyServiceToChangePageComponent } from './components/exchange-of-service-components/choose-my-service-to-change-page/choose-my-service-to-change-page.component';
import { ChooseTimeOfServiceFormComponent } from './components/exchange-of-service-components/choose-time-of-service-form/choose-time-of-service-form.component';
import { ChangingProposalsPageComponent } from './components/exchange-of-service-components/changing-proposals-page/changing-proposals-page.component';
import { SmallPostComponent } from './components/exchange-of-service-components/small-post/small-post.component';

import { MessagingCardsListComponent } from './components/messenger-components/messaging-cards-list/messaging-cards-list.component';
import { MessagingCardComponent } from './components/messenger-components/messaging-card/messaging-card.component';
import { MessagingAreaComponent } from './components/messenger-components/messaging-area/messaging-area.component';
import { MessageBlockComponent } from './components/messenger-components/message-block/message-block.component';
import { SendMessageFormComponent } from './components/messenger-components/send-message-form/send-message-form.component';

import { AuthorizationService } from './services/authorization.service';
import { RequestService } from './services/request.service';
import { MessengerSignalRService } from './services/messenger-signalR.service';
import { IoTSignalRService } from './services/IoT-signalR.service';
import { LoggerService } from './services/logger.service';

// определение маршрутов
const appRoutes: Routes = [
  { path: '', component: TimelinePageComponent },
  { path: 'addPostPage', component: AddPostPageComponent },
  { path: 'usersPage', component: UsersPageComponent },
  { path: 'userPage/:userName', component: UserPageComponent },
  { path: 'messengerPage', component: MessengerPageComponent },
  { path: 'accountPage', component: AccountPageComponent },
  { path: 'postPage/:id', component: PostPageComponent },
  { path: 'postPage/:id', component: PostPageComponent },
  { path: 'chooseMyServiceToChange/:id', component: ChooseMyServiceToChangePageComponent },
  { path: 'changingProposalsPage/:direction', component: ChangingProposalsPageComponent },
  { path: '**', component: NotFoundPageComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    TimelinePageComponent,
    UsersPageComponent,
    AccountPageComponent,
    NotFoundPageComponent,
    FooterComponent,
    AuthorizationFormComponent,
    PostComponent,
    AddPostPageComponent,
    PostPageComponent,
    EditPostFormComponent,
    UserPageComponent,
    SearchGeomarkerFormComponent,
    ChooseMyServiceToChangePageComponent,
    MessengerPageComponent,
    MessagingCardsListComponent,
    MessagingCardComponent,
    MessagingAreaComponent,
    MessageBlockComponent,
    SendMessageFormComponent,
    ChooseTimeOfServiceFormComponent,
    ChangingProposalsPageComponent,
    SmallPostComponent,
  ],
  imports: [
    BrowserModule, FormsModule, RouterModule.forRoot(appRoutes), GoogleMapsModule
  ],
  providers: [AuthorizationService, RequestService, LoggerService, MessengerSignalRService, IoTSignalRService],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
