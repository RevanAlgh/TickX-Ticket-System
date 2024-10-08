import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamMembersListComponent } from './team-members-list.component';

describe('TeamMembersListComponent', () => {
  let component: TeamMembersListComponent;
  let fixture: ComponentFixture<TeamMembersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamMembersListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TeamMembersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
