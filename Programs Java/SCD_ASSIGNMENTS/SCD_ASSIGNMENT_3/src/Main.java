import Controller.ResourceController;
import Controller.StrategyController;
import Controller.TaskController;
import Model.Resource;
import Model.Skill;
import Model.skillSet;
import Model.Task;
import View.ResourceView;
import View.StrategyView;
import View.TaskView;

import javax.swing.*;
import java.awt.*;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;

class AdditionForm {
	private JFrame frame;
	private JPanel panel;

	public AdditionForm() {
		frame = new JFrame("Resource and Task Allocator");
		frame.setSize(850, 700);
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		panel = new JPanel();
		panel.setLayout(new GridLayout(3, 1)); // 2 rows, 1 column

		frame.add(panel);
	}

	public void addPanel(JPanel panel) {
		this.panel.add(panel);
	}

	public void showView() {
		panel.revalidate();
		panel.repaint();

		frame.setVisible(true);
		frame.setResizable(false);
	}
}


public class Main {
	public static void main(String[] args) {
		ArrayList<skillSet> resources = new ArrayList<>();
		addPreResource(resources);
		ResourceView resourceView = new ResourceView();
		ResourceController resourceController = new ResourceController(resourceView, resources);

		ArrayList<skillSet> tasks = new ArrayList<>();
		addPreTasks(tasks);
		TaskView taskView = new TaskView();
		TaskController taskController = new TaskController(taskView, tasks);

		StrategyView strategyView = new StrategyView();
		StrategyController strategyController = new StrategyController(strategyView, tasks, resources);

		AdditionForm mainView = new AdditionForm();
		mainView.addPanel(resourceView.getMainPanel());
		mainView.addPanel(taskView.getMainPanel());
		mainView.addPanel(strategyView.getMainPanel());

		mainView.showView();
	}

	static void addPreResource(ArrayList<skillSet> resources) {

		skillSet ahmed = new Resource("Ahmed");
		ahmed.addSkill(new Skill("c", 2));
		ahmed.addSkill(new Skill("c++", 3));
		ahmed.addSkill(new Skill("java", 1));
		resources.add(ahmed);

		skillSet ayesha = new Resource("Ayesha");
		ayesha.addSkill(new Skill("c", 2));
		ayesha.addSkill(new Skill("c++", 2));
		ayesha.addSkill(new Skill("assembly", 2));
		resources.add(ayesha);

		skillSet ali = new Resource("Ali");
		ali.addSkill(new Skill("c++", 3));
		ali.addSkill(new Skill("java", 3));
		resources.add(ali);

		skillSet salman = new Resource("Salman");
		salman.addSkill(new Skill("java", 4));
		salman.addSkill(new Skill("javascript", 2));
		salman.addSkill(new Skill("python", 2));
		resources.add(salman);

		skillSet sara = new Resource("Sara");
		sara.addSkill(new Skill("python", 3));
		sara.addSkill(new Skill("javascript", 2));
		resources.add(sara);
	}

	static void addPreTasks(ArrayList<skillSet> tasks) {

		skillSet webDevelopment = new Task("Web Development");
		webDevelopment.addSkill(new Skill("javascript", "e"));
		webDevelopment.addSkill(new Skill("java", "i"));
		tasks.add(webDevelopment);

		skillSet dataAnalytics = new Task("Data Analytics");
		dataAnalytics.addSkill(new Skill("python", "e"));
		dataAnalytics.addSkill(new Skill("javascript", "i"));
		tasks.add(dataAnalytics);

		skillSet systemProgramming = new Task("System Programming");
		systemProgramming.addSkill(new Skill("c", "i"));
		systemProgramming.addSkill(new Skill("c++", "i"));
		systemProgramming.addSkill(new Skill("assembly", "i"));
		tasks.add(systemProgramming);
	}
//	static void addPreResource
}