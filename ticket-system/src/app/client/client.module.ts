import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClientRoutingModule } from './client-routing.module';
import { StatusPipe } from '../shared/pipes/status.pipe';

@NgModule({
  declarations: [],
  imports: [CommonModule, ClientRoutingModule],
})
export class ClientModule {}
