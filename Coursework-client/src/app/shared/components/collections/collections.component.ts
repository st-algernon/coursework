import { Component, Input, OnInit } from '@angular/core';
import { ShortCollection } from '../../interfaces';

@Component({
  selector: 'app-collections',
  templateUrl: './collections.component.html',
  styleUrls: ['./collections.component.scss']
})
export class CollectionsComponent implements OnInit {

  @Input() collections: ShortCollection[] = [];
  @Input() pageSize?: number;
  selectedCollections?: ShortCollection[];

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    this.selectedCollections = this.collections.slice(0, this.pageSize ?? this.collections.length);
  }
}
