import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './modules/material.module';
import { DndComponent } from './components/dnd/dnd.component';
import { ReactiveFormsModule } from '@angular/forms';
import { FormatBytesPipe } from './pipes/format-bytes.pipe';
import { RouterModule } from '@angular/router';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { CollectionFormComponent } from './components/collection-form/collection-form.component';
import { AddItemDialogComponent } from './components/add-item-dialog/add-item-dialog.component';
import { StringFieldComponent } from './components/field-types/string-field.component';
import { TypeFieldComponent } from './components/field-types/type-field.component';
import { NgxdModule } from '@ngxd/core';
import { TextFieldComponent } from './components/field-types/text-field.component';
import { NumericalFieldComponent } from './components/field-types/numerical-field.component';
import { DateFieldComponent } from './components/field-types/date-field.component';
import { CheckboxFieldComponent } from './components/field-types/checkbox-field.components';
import { ItemFormComponent } from './components/item-form/item-form.component';
import { EditItemDialogComponent } from './components/edit-item-dialog/edit-item-dialog.component';
import { DisplayFieldPipe } from './pipes/display-field.pipe';
import { ItemsComponent } from './components/items/items.component';
import { CollectionsComponent } from './components/collections/collections.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';
import { LayoutModule } from '@angular/cdk/layout';

export function httpTranslateLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    DndComponent,
    FormatBytesPipe,
    NotFoundComponent,
    CollectionFormComponent,
    AddItemDialogComponent,
    StringFieldComponent,
    TextFieldComponent,
    NumericalFieldComponent,
    DateFieldComponent,
    CheckboxFieldComponent,
    TypeFieldComponent,
    ItemFormComponent,
    EditItemDialogComponent,
    DisplayFieldPipe,
    ItemsComponent,
    CollectionsComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule,
    ReactiveFormsModule,
    NgxdModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: httpTranslateLoaderFactory,
        deps: [HttpClient]
      }
    }),
    LayoutModule
  ],
  exports: [
    MaterialModule,
    DndComponent,
    CollectionFormComponent,
    DisplayFieldPipe,
    ItemsComponent,
    CollectionsComponent,
    TranslateModule
  ],
  providers: []
})
export class SharedModule { }
