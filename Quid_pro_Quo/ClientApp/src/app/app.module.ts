import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';
//import { GoogleMapsModule } from '@angular/google-maps';

import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { TimelinePageComponent } from './components/timeline-page/timeline-page.component';
import { UsersPageComponent } from './components/users-page/users-page.component';
import { UserPageComponent } from './components/user-page/user-page.component';
import { AccountPageComponent } from './components/account-page/account-page.component';
import { PostPageComponent } from './components/post-page/post-page.component';
import { NotFoundPageComponent } from './components/not-found-page/not-found-page.component';
import { FooterComponent } from './components/footer/footer.component';
import { AuthorizationFormComponent } from './components/authorization-form/authorization-form.component';
import { AddPostPageComponent } from './components/add-post-page/add-post-page.component';
import { PostComponent } from './components/post/post.component';
import { EditPostFormComponent } from './components/edit-post-form/edit-post-form.component';

import { AuthorizationService } from './services/authorization.service';
import { RequestService } from './services/request.service';

// определение маршрутов
const appRoutes: Routes = [
  { path: '', component: TimelinePageComponent },
  { path: 'usersPage', component: UsersPageComponent },
  { path: 'userPage/:userName', component: UserPageComponent },
  { path: 'accountPage', component: AccountPageComponent },
  { path: 'postPage/:id', component: PostPageComponent },
  { path: 'addPostPage', component: AddPostPageComponent },
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
  ],
  imports: [
    BrowserModule, FormsModule, RouterModule.forRoot(appRoutes), //GoogleMapsModule
  ],
  providers: [ AuthorizationService, RequestService ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
