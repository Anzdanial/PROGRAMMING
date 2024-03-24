
//implement random obstacle generation afterwards
//make the grid generation dynamic in size afterwards
//make the grid template focused afterwards
//use google maps api to create maps routing
//add a boxing grid with borders into the implementation by understanding how grid are implementated in libraries
//class mapCreation{
//	private int array[][];
//	private int rows, columns;
//
//}
//public class Main {
//	public static void main(String[] args) {
//
//	}
//}

import javafx.application.Application;
import javafx.scene.Scene;
import javafx.scene.layout.GridPane;
import javafx.scene.paint.Color;
import javafx.scene.shape.Rectangle;
import javafx.stage.Stage;

class GridMapVisualization extends Application {
	private static final int EMPTY = 0;
	private static final int OBSTACLE = 1;
	private static final int ROAD = 2;
	private static final int START = 3;
	private static final int DESTINATION = 4;

	// Define grid size
	private static final int GRID_SIZE = 40;

	// Bitmap representing the grid map
	private int[][] bitmap;
	private static final int CELL_SIZE = 20;

	@Override
	public void start(Stage primaryStage) {
		GridPane gridPane = new GridPane();

		// Create rectangles representing cells
		for (int i = 0; i < GRID_SIZE; i++) {
			for (int j = 0; j < GRID_SIZE; j++) {
				Rectangle cell = new Rectangle(CELL_SIZE, CELL_SIZE);
				cell.setStroke(Color.BLACK);
				cell.setFill(Color.WHITE); // Default color for roads

				// Set colors based on cell type
				switch (bitmap[i][j]) {
					case OBSTACLE:
						cell.setFill(Color.BLACK);
						break;
					case START:
						cell.setFill(Color.GREEN);
						break;
					case DESTINATION:
						cell.setFill(Color.RED);
						break;
				}

				gridPane.add(cell, j, i); // Add cell to grid pane
			}
		}

		Scene scene = new Scene(gridPane, GRID_SIZE * CELL_SIZE, GRID_SIZE * CELL_SIZE);
		primaryStage.setScene(scene);
		primaryStage.setTitle("Grid Map Visualization");
		primaryStage.show();
	}

	public static void main(String[] args) {
		launch(args);
	}
}
