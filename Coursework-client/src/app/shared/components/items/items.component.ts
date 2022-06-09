import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Subscription } from 'rxjs';
import { ItemsClient, ShortItemVm } from '../../services/api.service';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.scss']
})
export class ItemsComponent implements OnInit, OnChanges {

  @Input() items: ShortItemVm[] = [];
  @Input() pageSize?: number;
  selectedItems?: ShortItemVm[];

  constructor(
    private itemsClient: ItemsClient
  ) { }

  ngOnChanges(): void {
    this.selectedItems = this.items.slice(0, this.pageSize ?? this.items.length);
  }

  ngOnInit(): void {
  }
}
