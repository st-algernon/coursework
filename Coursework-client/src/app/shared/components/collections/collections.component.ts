import { Component, Input, OnInit } from '@angular/core';
import { ShortCollectionVm } from '../../services/api.service';

@Component({
  selector: 'app-collections',
  templateUrl: './collections.component.html',
  styleUrls: ['./collections.component.scss']
})
export class CollectionsComponent implements OnInit {

  @Input() collections: ShortCollectionVm[] = [];
  @Input() pageSize?: number;
  selectedCollections?: ShortCollectionVm[];

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    this.selectedCollections = this.collections.slice(0, this.pageSize ?? this.collections.length);
  }
}
