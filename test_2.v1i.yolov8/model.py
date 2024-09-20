from ultralytics import YOLO

# Load a model
# model = YOLO("data.yaml")  # build a new model from YAML
model = YOLO("yolov8s.pt")  # load a pretrained model (recommended for training)
# model = YOLO("data.yaml").load("yolov8s.pt")  # build from YAML and transfer weights

# Train the model
results = model.train(data="./data.yaml", epochs=10, imgsz=640)