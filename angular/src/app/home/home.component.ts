import { AuthService, LocalizationPipe } from '@abp/ng.core';
import { Component, inject } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { DynamicFormComponent, FormFieldConfig } from '@abp/ng.components/dynamic-form';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  imports: [RouterModule],
})
export class HomeComponent {

}
