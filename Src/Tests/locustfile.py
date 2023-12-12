from locust import HttpUser, task, between

class Tests(HttpUser):
    # wait_time = between(1, 3)  

    def on_start(self):
        # Perform login and store the authentication cookie
        response = self.client.get("/User/login/false")
        if response.status_code == 200:
            self.client.cookies.update(response.cookies)
            
    # @task
    # def get_resources(self):
    #     response = self.client.get("/Resources")

    @task
    def post_resource(self):
        response = self.client.post("/Resources", json={
            "name": "test",
            "description": "my descccc",
            "source": "www.source.nl",
        })