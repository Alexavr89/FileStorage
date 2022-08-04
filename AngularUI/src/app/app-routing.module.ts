import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdministrationComponent } from './administration/administration.component';
import { AdministrationGuard } from './administration/administration.guard';
import { EdituserComponent } from './administration/edituser/edituser.component';
import { FilesComponent } from './files/files.component';
import { FilesGuard } from './files/files.guard';
import { PrivateComponent } from './files/private/private.component';
import { PublicComponent } from './files/public/public.component';
import { RegistrationComponent } from './registration/registration.component';

const routes: Routes = [
  {path: 'registration', component: RegistrationComponent},
  {path: 'administration', component: AdministrationComponent, canActivate: [AdministrationGuard]},
  {path: 'files', component: FilesComponent, canActivate:[FilesGuard]},
  {path:'administration/edituser', component:EdituserComponent},
  {path:'files/private', component:PrivateComponent},
  {path: 'files/public', component:PublicComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [AdministrationGuard, FilesGuard]
})
export class AppRoutingModule { }
