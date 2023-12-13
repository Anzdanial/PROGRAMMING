package Controller;

import Controller.Strategies.ExactMatch;
import Controller.Strategies.MatchingStrategy;
import Controller.Strategies.SkillOnlyMatch;
import Model.skillSet;
import View.StrategyView;

import javax.swing.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;

public class StrategyController {
    MatchingStrategy matchingStrategy;
    StrategyView strategyView;
    ArrayList<skillSet> resources;
    ArrayList<skillSet> tasks;

    ArrayList<String> matchedTalents;


    public StrategyController(StrategyView view, ArrayList<skillSet> tasks, ArrayList<skillSet> resources) {
        strategyView = view;
        this.resources = resources;
        this.tasks = tasks;
        initialize();
    }

    public void initialize(){
        matchedTalents = new ArrayList<>();
        refreshView();
        addListeners();
    }

    public void refreshView() {
        // Refresh the resource list based on the current resources data
        DefaultListModel<Object> model = new DefaultListModel<>();
        for (String t : matchedTalents) {
            model.addElement(t);
        }
        strategyView.getMatchedTalentsList().setModel(model);
    }

    protected void addListeners() {
        strategyView.getGenerateButton().addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                matchingStrategy = strategyView.getExactMatchButton().isSelected() ? new ExactMatch(tasks, resources) :
                                strategyView.getSkillOnlyMatchButton().isSelected() ? new SkillOnlyMatch(tasks, resources) : null;
                System.out.println(matchingStrategy == null ? "No strategy chosen!\n" : "Printing Lists: \n");
                matchingStrategy.PrintMatches();
                DefaultListModel<Object> matchedModel = new DefaultListModel<>();
                ArrayList<Object> matchingList =  matchingStrategy.ListMatches();
                for(Object string : matchingList){
                    matchedModel.addElement(string);
                }
                strategyView.getMatchedTalentsList().setModel(matchedModel);
            }
        });
    }

}
