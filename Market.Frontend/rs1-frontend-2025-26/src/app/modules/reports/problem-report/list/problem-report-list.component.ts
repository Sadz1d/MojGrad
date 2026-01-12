import { Component, OnInit } from '@angular/core';
import { ProblemReportService } from '../../../../services/problem-report-service';

@Component({
  selector: 'app-problem-report-list',
  templateUrl: './problem-report-list.component.html'
})
export class ProblemReportListComponent implements OnInit {

  reports: any[] = [];
  page = 1;
  pageSize = 10;
  total = 0;

  constructor(private reportService: ProblemReportService) {}

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(): void {
    this.reportService.getPaged(this.page, this.pageSize)
      .subscribe(res => {
        this.reports = res.items;
        this.total = res.total;
      });
  }

  nextPage(): void {
    if (this.page * this.pageSize < this.total) {
      this.page++;
      this.loadReports();
    }
  }

  prevPage(): void {
    if (this.page > 1) {
      this.page--;
      this.loadReports();
    }
  }
}



