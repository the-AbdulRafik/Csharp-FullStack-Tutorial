export interface Project {
  id: string;
  title: string;
  year: number;
  category: 'AI & Agents' | 'Agent Runtime' | 'Full-Stack';
  summary: string;
  technologies: string[];
  highlights: string[];
  featured: boolean;
  githubUrl?: string;
  liveDemoUrl?: string;
}

export interface SkillGroup {
  category: string;
  skills: string[];
}

export interface ExperienceItem {
  role: string;
  company: string;
  location: string;
  period: string;
  highlights: string[];
}

export interface EducationItem {
  degree: string;
  institution: string;
  university: string;
  location: string;
  period: string;
  score: string;
}

export interface AchievementItem {
  award: string;
  competition: string;
  details: string;
  date: string;
}

export interface ContactMessage {
  name: string;
  email: string;
  subject: string;
  message: string;
  urgency: 'normal' | 'high';
}

export interface PortfolioStats {
  status: string;
  lastUpdated: string;
  activeProjectsCount: number;
  skillsCount: number;
  hackathonWins: number;
  systemHealth: string;
}
