import { Layout } from "@/components/layout/Layout";
import { TestService } from "@/services/openapi";

export default function Test() {
    return (
        <Layout>
            <button className={"m-4 p-2 bg-black"} onClick={() => TestService.testProgressTask()}>Test Task</button>
        </Layout>
    )
}