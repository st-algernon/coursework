import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Subscription } from 'rxjs';
import { Item, ShortItem } from 'src/app/shared/interfaces';
import { ItemsService } from 'src/app/shared/services/items.service';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.scss']
})
export class ItemsComponent implements OnInit, OnChanges {

  @Input() items: ShortItem[] = [];
  @Input() pageSize?: number;
  selectedItems?: ShortItem[];

  constructor(
    private itemsService: ItemsService
  ) { }

  ngOnChanges(): void {
    this.selectedItems = this.items.slice(0, this.pageSize ?? this.items.length);
  }

  ngOnInit(): void {
  }
}
