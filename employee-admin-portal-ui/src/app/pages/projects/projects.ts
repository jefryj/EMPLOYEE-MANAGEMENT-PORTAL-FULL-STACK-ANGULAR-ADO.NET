import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Project, ProjectDto } from '../../models/project';
import { ProjectService } from '../../services/project';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './projects.html',
  styleUrl: './projects.css'
})
export class Projects implements OnInit {
  projects: Project[] = [];
  roleId = Number(localStorage.getItem('roleId'));
  projectName = '';
  editingId: number | null = null;
  errorMessage = '';

  constructor(private projectService: ProjectService) { }

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.projectService.getProjects().subscribe({
      next: (response) => {
        this.projects = response;
        this.errorMessage = '';
      },
      error: (error) => {
        console.error(error);
        this.errorMessage = 'Unable to load projects.';
      }
    });
  }

  saveProject(): void {
    if (this.roleId !== 1 || !this.projectName.trim()) {
      return;
    }

    const project: ProjectDto = { projectName: this.projectName.trim() };
    const request = this.editingId === null ? 
    this.projectService.addProject(project) : this.projectService.updateProject(this.editingId, project);

    request.subscribe({
      next: () => {
        this.projectName = '';
        this.editingId = null;
        this.loadProjects();
      },
      error: (error) => {
        console.error(error);
        this.errorMessage = 'Unable to save project.';
      }
    });
  }

  editProject(project: Project): void {
    if (this.roleId !== 1) {
      return;
    }

    this.editingId = project.id;
    this.projectName = project.projectName;
  }

  cancelEdit(): void {
    this.editingId = null;
    this.projectName = '';
  }

  deleteProject(id: number): void {
    if (this.roleId !== 1 || !confirm('Are you sure you want to delete this project?')) {
      return;
    }

    this.projectService.deleteProject(id).subscribe({
      next: () => this.loadProjects(),
      error: (error) => {
        console.error(error);
        this.errorMessage = 'Unable to delete project.';
      }
    });
  }
}