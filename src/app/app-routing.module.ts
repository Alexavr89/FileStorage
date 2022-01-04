import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdministrationComponent } from './administration/administration.component';
import { EdituserComponent } from './administration/edituser/edituser.component';
import { FilesComponent } from './files/files.component';
import { RegistrationComponent } from './registration/registration.component';

const routes: Routes = [
  {path: 'registration', component: RegistrationComponent},
  {path: 'administration', component: AdministrationComponent},
  {path: 'files', component: FilesComponent},
  {path:'administration/edituser', component:EdituserComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
